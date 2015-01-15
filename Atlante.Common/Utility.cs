using System;
using System.Globalization;

namespace Atlante.Common
{
    public static class Utility
    {
        public static IMessages MapDisk(string sharedarea, string username, string password, string mappeddisk)
        {
            Messages messages = new Messages();

            //TaskControllerStatus returnStatus = TaskControllerStatus.Stopped;
            string command = string.Format(CultureInfo.InvariantCulture, @"use {0} {1} {2} /user:{3}", mappeddisk, sharedarea, password, username);
            /*string error=string.Empty;
            string message=string.Empty;*/

            try
            {
                //messages = ExecuteShellCommand( "net", command );
                //ExecuteShellCommand("net", command, ref message, ref error);
            }
            catch (Exception e)
            {
                messages.AddException(e);
            }

            return messages;
        }

        public static void UnmapDisk(string mappeddisk)
        {
            throw new NotImplementedException();
        }
    }
}
