namespace bg.crm.integration.application.dtos.models.pagination
{
    public class PaginationFilterResponse<T>
    {
        public List<T> data { get; set; }
        public PaginationModel pagination { get; set; }

        public PaginationFilterResponse()
        {
            data = new List<T>();
            pagination = new PaginationModel();
        }
    }

}