namespace EasyParking.Core.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string name, object key)
            : base($"La entidad \"{name}\" ({key}) no fue encontrada.")
        {
        }
    }
} 