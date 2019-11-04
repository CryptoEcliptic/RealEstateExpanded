using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HomeHunter.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeHunter.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private const string FieldIsRequiredErrorMessage = "Полето {0} e задължително!";
        private const string PasswordRequirementsErrorMessage = "Паролатата трябва да бъде от поне {2} и да не надвишава {1} символа.";
        private const string ConfirmPasswordErrorMessage = "Данните в полета \"Парола\" и \"Потвърдете паролата\" трябва да съвпадат.";
        private const string ValidEmailErrorMessage = "Моля, въведете валиден Email адрес";

        private readonly UserManager<HomeHunterUser> _userManager;

        public ResetPasswordModel(UserManager<HomeHunterUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = FieldIsRequiredErrorMessage)]
            [EmailAddress(ErrorMessage = ValidEmailErrorMessage)]
            public string Email { get; set; }

            [Required(ErrorMessage = FieldIsRequiredErrorMessage)]
            [StringLength(100, ErrorMessage = PasswordRequirementsErrorMessage, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Нова парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърдете паролата")]
            [Compare("Password", ErrorMessage = ConfirmPasswordErrorMessage)]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = code
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
