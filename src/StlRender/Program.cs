namespace ModelRenderer
{
	class Program
	{
		static void Main(string[] args)
		{
			using (GameWindow game = new GameWindow())
			{
				game.Run();
			}
		}
	}
}
