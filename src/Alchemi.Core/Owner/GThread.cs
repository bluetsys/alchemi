#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GThread.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2003
 * Copyright    :   Copyright � 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 * 
 */
#endregion

using System;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a "thread" that can be run on a remote grid node
	/// </summary>
    [Serializable]
    public abstract class GThread : MarshalByRefObject
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
        
        bool _Failed = false;


        

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

        #region Property - Id
        private int _Id = -1;
        /// <summary>
        /// Gets the id of the grid thread
        /// </summary>
        public int Id
        {
            get { return _Id; }
        } 
        #endregion


		/// <summary>
		/// Sets the id of the grid thread
		/// </summary>
		/// <param name="id"></param>
        protected internal void SetId(int id)
        {
            _Id = id;
        }


        #region Property - WorkingDirectory
        [NonSerialized]
        string _WorkingDirectory = ""; // remote
        /// <summary>
        /// Gets the working directory of the grid thread
        /// </summary>
        protected string WorkingDirectory
        {
            get { return _WorkingDirectory; }
        } 
        #endregion


		/// <summary>
		/// Sets the working directory of the grid thread
		/// </summary>
		/// <param name="workingDirectory">the directory name to set as the working directory</param>
        public void SetWorkingDirectory(string workingDirectory)
        {
            _WorkingDirectory = workingDirectory;
        }

		/// <summary>
		/// Sets the thread state to failed
		/// </summary>
		/// <param name="failed">name indicating whether to set the thread to failed</param>
        public void SetFailed(bool failed)
        {
            _Failed = failed;
        }


        #region Property - Application
        [NonSerialized]
        GApplication _Application = null; // local
        /// <summary>
        /// Gets the application to which this grid thread belongs
        /// </summary>
        public GApplication Application
        {
            get { return _Application; }
        } 
        #endregion


		/// <summary>
		/// Sets the application to which this grid thread belongs
		/// </summary>
		/// <param name="application"></param>
        public void SetApplication(GApplication application)
        {
            _Application = application;
        }


        #region Property - Priority
        [NonSerialized]
        int _Priority = 5; // local
        /// <summary>
        /// Gets or sets the grid thread priority
        /// </summary>
        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        } 
        #endregion


		/// <summary>
		/// Gets the state of the grid thread
		/// </summary>
        public virtual ThreadState State
        {
            get { return _Application.GetThreadState(this); }
        }


        /*
        public Exception RemoteExecutionException
        {
            get { return _RemoteExecutionException; }
            set { _RemoteExecutionException = name; }
        }
        */

		public void StartThread()
		{
			try
			{
				Start();
			}
			catch(Exception e)
			{
				// can't de-serialize custom exceptions on another app domain, 
				// so catch wrap and re-thorw exceptions.
				throw new GThreadException(e);
			}
		}

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Starts the execution of the thread on the remote node.
		/// This method is to be implemented by subclasses to include code 
		/// which is actually executed on the executor.
		/// </summary>
        public abstract void Start();
        
        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Aborts this grid thread
		/// </summary>
        public void Abort()
        {
            _Application.AbortThread(this);
        }
    }
}

