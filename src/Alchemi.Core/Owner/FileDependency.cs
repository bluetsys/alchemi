#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   FileDependency.cs
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
using System.IO;
using System.Runtime.Serialization;

using Alchemi.Core.Utility;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// The FileDependency abstract class defines the members that need to exist in sub classes that are used to implement 
	/// "File" Dependencies. A file dependency represents a single file on which the grid application depends for input.
	/// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(Alchemi.Core.Owner.ModuleDependency))]
    [KnownType(typeof(Alchemi.Core.Owner.EmbeddedFileDependency))]
    public abstract class FileDependency
    {

        #region Property - FileName
        [DataMember]
        private string _fileName;
        /// <summary>
        /// The filename of the FileDependency
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        } 
        #endregion


        #region Constructor
        /// <summary>
        /// Creates an instance of the FileDependency
        /// </summary>
        /// <param name="fileName">name of the file</param>
        protected FileDependency(string fileName)
        {
            _fileName = fileName;
        } 
        #endregion


		/// <summary>
		/// Unpacks the file to the specified location
		/// </summary>
		/// <param name="fileLocation">location and file name to unpack the file</param>
        public abstract void Unpack(string fileLocation);


        /// <summary>
        /// Unpacks the file to the specified folder.
        /// The current file name is appended to the given folder name.
        /// </summary>
        /// <remarks>
        /// The FileName property might contain some folder structures as well. 
        /// This can be used to reproduce a folder structure.
        /// </remarks>
        /// <param name="targetFolder">Folder where the current file will be unpacked</param>
        public void UnpackToFolder(string targetFolder)
        {
            string targetFileName = Path.Combine(targetFolder, FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));
            Unpack(targetFileName);
        }

    }
}