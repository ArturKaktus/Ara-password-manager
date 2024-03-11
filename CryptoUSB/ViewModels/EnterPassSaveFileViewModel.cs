using System;
using System.Collections.Generic;
using ReactiveUI;

namespace CryptoUSB.ViewModels
{
	public class EnterPassSaveFileViewModel : ReactiveObject
	{
		public string Password { get; set; }
		public string RePassword { get; set; }
        private bool _IsEnableButton;
        public bool IsEnableButton { get => _IsEnableButton; set { this.RaiseAndSetIfChanged(ref _IsEnableButton, value); } }
        public EnterPassSaveFileViewModel()
		{

		}
	}
}