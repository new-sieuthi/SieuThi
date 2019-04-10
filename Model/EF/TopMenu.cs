namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sieuthin.TopMenu")]
    public partial class TopMenu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(250)]
        [DisplayName("Tên")]
        public string Name { get; set; }


        [StringLength(250)]
        [DisplayName("Hình ảnh")]
        public string Image { get; set; }


        public DateTime? CreatedOn { get; set; }

        [StringLength(250)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        public bool? Status { get; set; }
    }
}
