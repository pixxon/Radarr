using FluentValidation;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Plex.Server
{
    public class PlexServerSettingsValidator : AbstractValidator<PlexServerSettings>
    {
        public PlexServerSettingsValidator()
        {
            RuleFor(c => c.Address).ValidAddress();
            RuleFor(c => c.MapFrom).NotEmpty().Unless(c => c.MapTo.IsNullOrWhiteSpace());
            RuleFor(c => c.MapTo).NotEmpty().Unless(c => c.MapFrom.IsNullOrWhiteSpace());
        }
    }

    public class PlexServerSettings : IProviderConfig
    {
        private static readonly PlexServerSettingsValidator Validator = new PlexServerSettingsValidator();

        [FieldDefinition(0, Label = "NotificationsSettingsAddress", HelpText = "NotificationsSettingsAddressHelpText")]
        [FieldToken(TokenField.HelpText, "NotificationsSettingsAddress", "serviceName", "Plex")]
        public string Address { get; set; } = "http://localhost:32400";

        [FieldDefinition(1, Label = "NotificationsPlexSettingsAuthToken", Type = FieldType.Textbox, Privacy = PrivacyLevel.ApiKey, Advanced = true)]
        public string AuthToken { get; set; }

        [FieldDefinition(2, Label = "NotificationsPlexSettingsAuthenticateWithPlexTv", Type = FieldType.OAuth)]
        public string SignIn { get; set; } = "startOAuth";

        [FieldDefinition(3, Label = "NotificationsSettingsUpdateLibrary", Type = FieldType.Checkbox)]
        public bool UpdateLibrary { get; set; } = true;

        [FieldDefinition(4, Label = "NotificationsSettingsUpdateMapPathsFrom", Type = FieldType.Textbox, Advanced = true, HelpText = "NotificationsSettingsUpdateMapPathsFromHelpText")]
        [FieldToken(TokenField.HelpText, "NotificationsSettingsUpdateMapPathsFrom", "serviceName", "Plex")]
        public string MapFrom { get; set; }

        [FieldDefinition(5, Label = "NotificationsSettingsUpdateMapPathsTo", Type = FieldType.Textbox, Advanced = true, HelpText = "NotificationsSettingsUpdateMapPathsToHelpText")]
        [FieldToken(TokenField.HelpText, "NotificationsSettingsUpdateMapPathsTo", "serviceName", "Plex")]
        public string MapTo { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
