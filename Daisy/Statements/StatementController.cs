namespace Ancestry.Daisy.Statements
{
    public class StatementController<T> 
    {
        public T Scope { get; set; }

        public dynamic Context { get; set; }
        public dynamic Attachments { get; set; }
    }
}
