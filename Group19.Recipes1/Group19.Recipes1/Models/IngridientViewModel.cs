namespace Group19.Recipes1.Models
{
    public class IngredientViewModel
    {
        public int Id { get; set; }

        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public MeasurementViewModel MeasurementViewModel { get; set; }
    }
}