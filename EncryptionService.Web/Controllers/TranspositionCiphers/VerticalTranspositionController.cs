﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using EncryptionService.Web.Configurations;
using EncryptionService.Core.Interfaces;
using EncryptionService.Web.Models;
using EncryptionService.Core.Models.TranspositionCiphers.VerticalTransposition;

namespace EncryptionService.Web.Controllers.TranspositionCiphers
{
	public class VerticalTranspositionController(
		IEncryptionService<VerticalTranspositionEncryptionResult, VerticalTranspositionKey, string>
		encryptionService,
		IOptions<EncryptionSettings> encryptionSettings) : Controller
	{
		readonly IEncryptionService<VerticalTranspositionEncryptionResult,
			VerticalTranspositionKey, string> _encryptionService = encryptionService;
		readonly EncryptionSettings _encryptionSettings = encryptionSettings.Value;

		public IActionResult Index() => View();

		[HttpPost]
		public IActionResult Index(
			EncryptionViewModel<VerticalTranspositionEncryptionResult> encryptionViewModel,
			string actionType)
		{
			if (!ModelState.IsValid)
				return View(encryptionViewModel);

			VerticalTranspositionKey key = _encryptionSettings.VerticalTranspositionKey;
			ViewData["Key"] = key.Key;
			VerticalTranspositionEncryptionResult encryptionResult;

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