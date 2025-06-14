using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects
{
    public sealed class ItemDescription : IEquatable<ItemDescription>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public ItemDescription(string title, string description)
        {
            Validate(title, description);
            Title = title;
            Description = description;
        }

        public override bool Equals(object? obj) => Equals(obj as ItemDescription);

        public bool Equals(ItemDescription? other)
        {
            if (other is null)
                return false;

            return Title == other.Title && Description == other.Description;
        }

        public override int GetHashCode() => Title.GetHashCode();

        public override string ToString() => Title.ToString();

        private void Validate(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException(DomainExceptionMessages.InvalidItemTitle);
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException(DomainExceptionMessages.InvalidItemDescription);
        }
    }
}
