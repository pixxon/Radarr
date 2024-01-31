using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Signal
{
    public class SignalSettingsValidator : AbstractValidator<SignalSettings>
    {
        public SignalSettingsValidator()
        {
            RuleFor(c => c.Address).ValidAddress();
            RuleFor(c => c.SenderNumber).NotEmpty();
            RuleFor(c => c.ReceiverId).NotEmpty();
        }
    }

    public class SignalSettings : IProviderConfig
    {
        private static readonly SignalSettingsValidator Validator = new ();

        [FieldDefinition(0, Label = "NotificationsSettingsAddress", HelpText = "NotificationsSettingsAddressHelpText")]
        [FieldToken(TokenField.HelpText, "NotificationsSettingsAddress", "serviceName",  "Signal")]
        public string Address { get; set; } = "http://localhost:8080";

        [FieldDefinition(1, Label = "NotificationsSignalSettingsSenderNumber", Privacy = PrivacyLevel.ApiKey, HelpText = "NotificationsSignalSettingsSenderNumberHelpText")]
        public string SenderNumber { get; set; }

        [FieldDefinition(2, Label = "NotificationsSignalSettingsGroupIdPhoneNumber", HelpText = "NotificationsSignalSettingsGroupIdPhoneNumberHelpText")]
        public string ReceiverId { get; set; }

        [FieldDefinition(3, Label = "Username", Privacy = PrivacyLevel.UserName, HelpText = "NotificationsSignalSettingsUsernameHelpText")]
        public string AuthUsername { get; set; }

        [FieldDefinition(4, Label = "Password", Type = FieldType.Password, Privacy = PrivacyLevel.Password, HelpText = "NotificationsSignalSettingsPasswordHelpText")]
        public string AuthPassword { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
