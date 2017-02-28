namespace SampleWebApi.Dto
{
    /// <summary>
    /// A customer
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The customers name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// </summary>
        public Customer()
        {
        }

        /// <summary>
        /// </summary>
        public Customer(string name)
        {
            Name = name;
        }
    }
}