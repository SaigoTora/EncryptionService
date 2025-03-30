﻿using EncryptionService.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Core.Models.BlockTransposition;
using EncryptionService.Core.Models;
using EncryptionService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EncryptionService.Core.Models.SloganEncryption;

namespace EncryptionService.Controllers
{
	public class SloganEncryption(
		IEncryptionService<EncryptionResult, SloganEncryptionKey, string> encryptionService,
		IOptions<EncryptionSettings> encryptionSettings)
		: Controller
	{
		readonly IEncryptionService<EncryptionResult, SloganEncryptionKey, string>
			_encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(EncryptionViewModel<EncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			SloganEncryptionKey key = _encryptionSettings.SloganEncryptionKey;
			EncryptionResult encryptionResult;

			if (actionType == "Encrypt")
			{
				encryptionResult = _encryptionService.Encrypt(encryptionViewModel.InputText!, key);
				encryptionViewModel.EncryptionResult = encryptionResult;
			}
			else if (actionType == "Decrypt")
			{
				encryptionResult = _encryptionService.Decrypt(
					encryptionViewModel.EncryptedInputText!, key);
				encryptionViewModel.DecryptionResult = encryptionResult;
			}

			return View(encryptionViewModel);
		}
	}
}
