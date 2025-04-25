namespace bg.crm.integration.application.dtos.models.pagination
{
    public class PaginationModel
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }
        public int Returned { get; set; }
    }
}