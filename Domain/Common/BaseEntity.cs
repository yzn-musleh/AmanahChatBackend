using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTimeOffset LastActionDate { get; set; }

        public Guid? ActionBy { get; set; }

        [Column("Metadata")]
        public string? MetadataAsString { get; set; }

        [NotMapped]
        public List<KeyValuePair<string, string>>? Metadata
        {
            get
            {
                return !string.IsNullOrEmpty(MetadataAsString) ?
                    JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>?>(MetadataAsString!)
                    : null;
            }
            set
            {
                MetadataAsString = JsonConvert.SerializeObject(value);
            }
        }


    }
}
