using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// This is a class that implements the ICommand interface
    /// </summary>
    class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="m_modal"></param>
        public NewFileCommand(IImageServiceModal m_modal)
        {
            this.m_modal = m_modal;
        }

        /// <summary>
        /// In this class implementation there is a call to the add file method
        /// of the IImageModal member
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// returns what AddFile returns
        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result);
        }
    }
}