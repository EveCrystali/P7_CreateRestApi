using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public interface IValidatable
    {
        void Validate();
    }
}
