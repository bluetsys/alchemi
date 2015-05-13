#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ManagerContainer.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright � 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au), and Krishna Nadiminti (kna@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion


using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.ServiceModel;
using ThreadState = Alchemi.Core.Owner.ThreadState;

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;
using Alchemi.Manager.Storage;
using Alchemi.Core.Utility;
using Alchemi.Core.EndPointUtils;

namespace Alchemi.Manager
{
	/// <summary>
	/// Event handler for status changes during Manager startup.
	/// </summary>
	public delegate void ManagerStartedEventHandler(object sender, EventArgs e);

	/// <summary>
	/// This class acts as a container for the manager and the applications,executors collections.
	/// </summary>
    public class ManagerContainer
    {		
		/// <summary>
		/// Manager Configuration
		/// </summary>
        private Configuration _Config = null;

        public Configuration Config
        {
            get { return _Config; }
            set { _Config = value; }
        }

		/// <summary>
		/// The configuration file used to set remoting parameters for the Manager.
		/// </summary>
        private string _RemotingConfigFile = "Alchemi.Manager.exe.config"; //this will be changed by the service/exec application and set to the right name.

        public string RemotingConfigFile
        {
            get { return _RemotingConfigFile; }
            set { _RemotingConfigFile = value; }
        }

		/// <summary>
		/// Specifies if the Manager is started.
		/// </summary>
        private bool _Started = false;

        public bool Started
        {
            get { return _Started; }
            set { _Started = value; }
        }

		//since starting and stopping may take a while, we better avoid calling start/stop twice,
		//when the process of start/stop is in progress
		private bool _Starting = false;
		private bool _Stopping = false;

		private IChannel _Chnl;

        private ServiceHost _sh;

		private Thread _InitExecutorsThread;

        private WatchDog watchdog = null;
        private Dispatcher dispatcher = null;

		private static readonly Logger logger = new Logger();

		/// <summary>
		/// This event is raised in response to the call to StartApplication, to notify the completion of the "StartApplication" call.
		/// </summary>
		public static event ManagerStartedEventHandler ManagerStartEvent;

        #region Constructor
        /// <summary>
		/// Creates an instance of the ManagerContainer.
		/// </summary>
		public ManagerContainer()
		{
			Started = false;
            dispatcher = new Dispatcher();
            watchdog = new WatchDog();
        }
        #endregion
       
        #region Method - Start
        /// <summary>
        /// Starts the Manager
        /// </summary>
        public void Start()
        {

            if (Started || _Starting)
                return;

            try
            {

                _Starting = true;

                if (Config == null)
                {
                    ReadConfig();
                }

                //See if there is any remoting end poit. 
                //There can be only one remoting end point.
                //See if there are any WCF end point. Thre can be more WCF end points.
                EndPointConfiguration remotingEpc = null;
                bool areAnyWcfEps = false;

                foreach (string key in Config.EndPoints.Keys)
                {
                    EndPointConfiguration epc = Config.EndPoints[key];
                    if (epc.RemotingMechanism == RemotingMechanism.TcpBinary)
                    {
                        if (remotingEpc != null)
                            throw new DoubleRemotingEndPointException("Cannot set two EndPoint where Rempting Mechanism is set to TcpBinary");

                        remotingEpc = epc;
                    }
                    else
                    {
                        areAnyWcfEps = true;
                    }
                }

                if (remotingEpc != null)
                    StartTcpBinary(remotingEpc);

                if (areAnyWcfEps)
                    StartWCF();


                logger.Debug("Configuring storage...");
                ManagerStorageFactory.CreateManagerStorage(Config);
                if (!ManagerStorageFactory.ManagerStorage().VerifyConnection())
                {
                    throw new Exception("Error connecting to manager storage. Please check manager log file for details.");
                }

                logger.Debug("Configuring internal shared class...");
                InternalShared common = InternalShared.GetInstance(Config);

                logger.Debug("Starting dispatcher thread");
                dispatcher.Start();

                logger.Info("Starting watchdog thread");
                watchdog.Start();

                //start a seperate thread to init-known executors, since this may take a while.
                _InitExecutorsThread = new Thread(new ThreadStart(InitExecutors));
                _InitExecutorsThread.Name = "InitExecutorsThread";
                _InitExecutorsThread.Start();

                Config.Serialize();

                Started = true;

                try
                {
                    if (ManagerStartEvent != null)
                        ManagerStartEvent(this, new EventArgs());
                }
                catch { }

            }
            catch (Exception ex)
            {
                Stop();
                logger.Error("Error Starting Manager Container", ex);
                throw ex;
            }
            finally
            {
                _Starting = false;
            }

        }
        #endregion

        #region Method - StartTcpBinary
        public void StartTcpBinary(EndPointConfiguration epc)
        {

            EndPoint ownEP = new EndPoint(epc.Port, RemotingMechanism.TcpBinary);

            logger.Debug("Configuring remoting...");

            RemotingConfiguration.Configure(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RemotingConfigFile), false);

            //TODO: for hierarchical grids
            //				RemoteEndPoint managerEP = null;
            //				if (Config.Intermediate)
            //				{
            //					managerEP = new RemoteEndPoint(
            //						Config.ManagerHost, 
            //						Config.ManagerPort, 
            //						RemotingMechanism.TcpBinary
            //						);
            //				}

            logger.Debug("Registering tcp channel on port: " + ownEP.Port);

            _Chnl = new TcpChannel(epc.Port);
            ChannelServices.RegisterChannel(_Chnl, false);

            //since this is a single call thing, thread safety isnt an issue

            logger.Debug("Registering well known service type");

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(GManager), "Alchemi_Node",
                WellKnownObjectMode.SingleCall);

            // TODO: hierarchical grids ignored until after v1.0.0
            /*
            _Dedicated = dedicated;
            _Id = id;

            if (Manager != null)
            {
                if (_Id == "")
                {
                    Log("Registering new executor ...");
                    _Id = Manager.Executor_RegisterNewExecutor(null, new ExecutorInfo);
                    Log("New ExecutorID = " + _Id);
                }

                try
                {
                    try
                    {
                        ConnectToManager();
                    }
                    catch (InvalidExecutorException)
                    {
                        Log("Invalid executor! Registering new executor ...");
                        _Id = Manager.Executor_RegisterNewExecutor(null, new ExecutorInfo);
                        Log("New ExecutorID = " + _Id);
                        ConnectToManager();
                    }
                }
                catch (ConnectBackException)
                {
                    Log("Couldn't connect as dedicated executor. Reverting to non-dedicated executor.");
                    _Dedicated = false;
                    ConnectToManager();
                }
            }
            */

        }
        #endregion

        #region Method - StartWCF
        public void StartWCF()
        {
            logger.Debug("Configuring WCF...");

            logger.Debug("Registering WCF as written in config file.");

            _sh = new ServiceHost(typeof(GManager), new Uri[] { });
            Alchemi.Core.Utility.WCFUtils.SetPublishingServiceHost(_sh);

            foreach (string key in Config.EndPoints.Keys)
            {
                EndPointConfiguration epc = Config.EndPoints[key];
                if (epc.RemotingMechanism != RemotingMechanism.TcpBinary)
                {
                    EndPoint ep = epc.GetEndPoint();
                    logger.Debug(String.Format("Registering WCF end point {0}...", ep.ToString()));
                    try
                    {

                        _sh.AddServiceEndpoint(typeof(IManager), Alchemi.Core.Utility.WCFUtils.GetWCFBinding(ep), ep.FullPublishingAddress);
                        logger.Debug("Success.");
                    }
                    catch (Exception ex)
                    {
                        logger.Debug(String.Format("Failed Registering WCF end point {0} with exception {1}", ep.ToString(), ex.ToString()));
                    }
                }
            }

            _sh.Open();

        }
        #endregion

        #region Method - ReadConfig
        /// <summary>
		/// Reads the Manager configuration from the Alchemi.Manager.config.xml file, 
		/// <br /> or gets the default configuration, if there is an error reading the file.
		/// </summary>
		public void ReadConfig()
		{			
			try
			{
				Config = Configuration.GetConfiguration();
			}
			catch //get default
			{
                Config = new Configuration();
			}
        }
        #endregion

        #region Methods - InitExecutors
        private void InitExecutors()
		{
			try
			{
				logger.Info("Initialising known executors...");
                (new MExecutorCollection()).Init();
				logger.Info("Initialising known executors...done");
			}
			catch(Exception ex)
			{
				logger.Debug("Exception in InitExecutors: ", ex);
			}
        }
        #endregion

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

        #region Method - Stop
        /// <summary>
		/// Stop the scheduler and watchdog threads, and shut down the manager.
		/// </summary>
        public void Stop()
        {
            try
            {
                if (!Started && !_Stopping)
                    return;

                _Stopping = true;

                if (_InitExecutorsThread != null)
                {
                    logger.Info("Stopping init-executors thread...");
                    _InitExecutorsThread.Abort();
                    _InitExecutorsThread.Join();
                }

                logger.Info("Stopping watchdog...");
                watchdog.Stop();

                logger.Info("Stopping dispatcher...");
                dispatcher.Stop();

                logger.Info("Cleaning up all apps...");
                //Cleanup();

                if (_Chnl != null)
                {
                    logger.Info("Unregistering the remoting channel...");
                    ChannelServices.UnregisterChannel(_Chnl);
                }

                if (_sh != null)
                {
                    if (_sh.State == CommunicationState.Opened)
                        _sh.Close();

                    if (_sh.State != CommunicationState.Closed)
                        _sh.Abort();

                    _sh = null;
                }

                Config.Serialize();

                Started = false;
            }
            finally
            {
                _Stopping = false;
            }
        }
        #endregion

        #region Method - Cleanup
        private void Cleanup()
		{
			try
			{
                Directory.Delete(InternalShared.Instance.DataRootDirectory, true);
			}
			catch (Exception e)
			{
				logger.Debug("Clean up error : ",e);
			}
        }
        #endregion

    }
}

