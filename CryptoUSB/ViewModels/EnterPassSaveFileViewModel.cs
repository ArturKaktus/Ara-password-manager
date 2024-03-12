using System;
using System.Collections.Generic;
using ReactiveUI;

namespace CryptoUSB.ViewModels
{
	public class EnterPassSaveFileViewModel : ReactiveObject
	{
		private string m_Password;
		private string m_RePassword;
		private string m_Path;

        public string Password 
		{ 
			get => m_Password;
            set => this.RaiseAndSetIfChanged(ref m_Password, value);
		}
		public string RePassword 
		{ 
			get => m_RePassword; 
			set
			{
                this.RaiseAndSetIfChanged(ref m_RePassword, value);
				if (m_Password == m_RePassword && !string.IsNullOrEmpty(m_Password))
					IsEnableButton = true;
            }
		}
		public string Path
		{
			get => m_Path;
			set => this.RaiseAndSetIfChanged(ref m_Path, value);
		}
        private bool _IsEnableButton;
        public bool IsEnableButton { get => _IsEnableButton; set { this.RaiseAndSetIfChanged(ref _IsEnableButton, value); } }
        public EnterPassSaveFileViewModel(string path)
		{
			Path = path;
		}
	}
}