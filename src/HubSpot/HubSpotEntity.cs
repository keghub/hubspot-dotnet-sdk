namespace HubSpot
{
    public abstract class HubSpotEntity<T>
    {
        protected HubSpotEntity(T entity)
        {
            InnerEntity = entity;
        }

        public T InnerEntity { get; }
    }
}