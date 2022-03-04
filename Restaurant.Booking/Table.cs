namespace RestaurantProject.Booking
{
    public class Table
    {
        public State State { get; private set; }
        public int SeatsCount { get; }
        public int Id { get; set; }

        public Table(int id)
        {
            Id = id;
            State = State.Free;
            SeatsCount = new Random((int)DateTime.Now.Ticks).Next(2, 5);
        }

        public bool SetState(State state)
        {
            if (state == State)
                return false;

            State = state;
            return true;
        }
    }
}
