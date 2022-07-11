namespace ServIT.Data.DTO.SubCategory
{
    public class SubCategoryDTO
    {
        public Guid Id { get; set; }
        public string SubCategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CreatedByUserID { get; set; }
        public Guid DeletedByUserID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }




}
