namespace Twitch_Bot_Builder
{
	public class Action
	{
		// type of command
		public byte type = 0;
		// the output of a typing command or keypress command (E.G You want to type hello the output variable is hello)
		public string output;
		// path to the file that you want to run
		public string path;
		// duration a key will be held if you opt for the key press command
		private float duration;
		// the point that your mouse will go to
		public int[] point = new int[2];

		// setup variables for typing command
		public void setTyping(string output)
		{
			type = 1;
			this.output = output;
		}

		//setup variables for key press commands
		public void setKeyPressing(string output, float duration)
		{
			type = 2;
			this.output = output;
			this.duration = duration;
		}

		// setup variabales for process commands
		public void setProcess(string path)
		{
			type = 3;
			this.path = path;
		}

		//setup variables for mouse commands
		public void setMousePos(int x, int y)
		{
			type = 4;
			point = new int[2] { x, y };
		}

		public void setMousePos(int[] point)
		{
			type = 4;
			this.point = point;
		}

		public string getMousePos()
		{
			return $"{point[0]}, {point[1]}";
		} 

		// transforming the types 
		public void setType(byte type)
		{
			this.type = type;
		}

		public string getType()
		{
			switch (type) {
				case 1:
					return "Typing";
				case 2:
					return "Key Pressing";
				case 3:
					return "Proccess";
				case 4:
					return "Mouse Position";
				default:
					return "Error Invalid Type";
			}
		}

		// returns the string that will be entered in the command line to be sent to python
		// the python code takes in word being typed and duration each letter will be held on to for
		public string getKeyPress()
		{
			return $"{output} {duration}";
		}

		public string getDuration()
		{
			return duration.ToString();
		}
	}
}