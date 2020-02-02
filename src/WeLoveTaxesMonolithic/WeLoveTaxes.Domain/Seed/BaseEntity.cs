using System;

namespace WeLoveTaxes.Domain.Seed
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; }

        public bool IsTransient()
        {
            return Id == default(Guid);
        }


        public override bool Equals(object obj)
        {
            if (!(obj is BaseEntity))
                return false;
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var parsedObj = (BaseEntity)obj;
            if (GetType() != parsedObj.GetType())
            {
                return false;
            }
            if (parsedObj.IsTransient() || IsTransient())
            {
                return false;
            }
            return parsedObj.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
