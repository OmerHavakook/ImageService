using System;

namespace ImageServiceInfrastructure.Event
{
    public class MobileCommandArgs : EventArgs
    {
        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pic"></param>
        public MobileCommandArgs(string name,byte[] pic)
        {
            this.Name = name;
            this.PicBytes = pic;

        }

        public string Name { get; } // string
        public byte[] PicBytes { get; } 
    }
}
