namespace Game
{
    public class HelloWorldModel : IHelloWorldModel
    {
        public string data { get; set; }

        public HelloWorldModel()
        {
            data = "Hello world!";
        }
    }
}