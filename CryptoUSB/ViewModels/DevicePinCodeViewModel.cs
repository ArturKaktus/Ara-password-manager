using System;
using System.Collections.Generic;
using ReactiveUI;

namespace CryptoUSB.ViewModels
{
	public class DevicePinCodeViewModel : ReactiveObject
	{
		private string m_PinCode;
		public string PinCode
		{
			get => m_PinCode;
			set => this.RaiseAndSetIfChanged(ref m_PinCode, value);
        }
        private bool _IsEnableButton;
        public bool IsEnableButton { get => _IsEnableButton; set { this.RaiseAndSetIfChanged(ref _IsEnableButton, value); } }
    }
}