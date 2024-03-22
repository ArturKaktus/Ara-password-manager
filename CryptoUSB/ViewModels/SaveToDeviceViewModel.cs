using System;
using System.Collections.Generic;
using ReactiveUI;

namespace CryptoUSB.ViewModels
{
	public class SaveToDeviceViewModel : ReactiveObject
	{
        public SaveToDeviceViewModel() { }
        private string m_PinCode;
        public string PinCode
        {
            get => m_PinCode;
            set
            {
                if (m_PinCode != value)
                {
                    this.RaiseAndSetIfChanged(ref m_PinCode, value);
                    IsEnableButton = !string.IsNullOrEmpty(m_PinCode);
                }
            }
        }
        private bool _IsEnableButton;
        public bool IsEnableButton { get => _IsEnableButton; set { this.RaiseAndSetIfChanged(ref _IsEnableButton, value); } }
    }
}