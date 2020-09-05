using Ardalis.GuardClauses;

namespace ApplicationCore.Entities
{
    public class Customer : BaseEntity
    {
        public string IdentityGuid { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Customer() { }

        public Customer(string identityGuid, string firstName, string lastName) : this()
        {
            Guard.Against.NullOrEmpty(identityGuid, nameof(identityGuid));
            Guard.Against.NullOrEmpty(firstName, nameof(firstName));
            Guard.Against.NullOrEmpty(lastName, nameof(lastName));

            IdentityGuid = identityGuid;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
