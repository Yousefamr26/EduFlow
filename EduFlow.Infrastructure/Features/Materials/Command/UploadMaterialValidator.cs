using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.Infrastructure.Features.Material.Command
{
    public class UploadMaterialValidator : AbstractValidator<UploadMaterialCommand>
    {
        public UploadMaterialValidator()
        {
            RuleFor(x => x.SessionId).GreaterThan(0);
            RuleFor(x => x.TeacherId).NotEmpty();

            RuleFor(x => x.Type)
                .Must(t => t == "File" || t == "Video");

            When(x => x.Type == "File", () =>
            {
                RuleFor(x => x.File).NotNull();

                RuleFor(x => x.File.Length)
                    .LessThanOrEqualTo(10 * 1024 * 1024);

                RuleFor(x => x.File.ContentType)
                    .Must(t =>
                        t == "application/pdf" ||
                        t == "application/msword" ||
                        t.Contains("wordprocessingml") ||
                        t.StartsWith("image/"));
            });

            When(x => x.Type == "Video", () =>
            {
                RuleFor(x => x.VideoUrl)
                    .NotEmpty()
                    .Must(url =>
                        url.Contains("youtube") ||
                        url.Contains("youtu.be") ||
                        url.Contains("drive.google"));
            });
        }
    }
}
