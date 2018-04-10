using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal m_modal)
        {
            this.m_modal = m_modal;
        }

        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result);
        }
    }
}
