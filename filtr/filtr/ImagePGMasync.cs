using System;
using System.Threading;
using System.Threading.Tasks;

namespace filtr
{
	class ImagePGMasync
	{
		public int Height { get; }
		public int Width { get; }
		public float[,] Pixels { get; private set; }
		private static float[,] newPixels { get; set; }

		public ImagePGMasync()
		{
			Height = 1026;
			Width = 1026;
			Pixels = new float[Height, Width];
		}

		public void Generate()
		{
			for (int i = 1; i < Height - 1; i++)
			{
				for (int j = 1; j < Width - 1; j++)
				{
					if ((j - 1) % 1024 < 512 && (i - 1) % 1024 < 512 || ((j - 1) % 1024 >= 512 && (i - 1) % 1024 >= 512))
					{
						Pixels[j, i] = 255;
					}
					else
					{
						Pixels[j, i] = 0;
					}

				}
			}
		}

		public void ConvolutionAsyncTask()
		{
			var tempPixels = new float[Height, Width];
			int numberOfTasks = 512;
			Task[] tasks = new Task[numberOfTasks];

			for (int k = 0; k < numberOfTasks; k++)
			{
				var t = new Task((param) =>
				{
					int blockSize = 1024 / numberOfTasks;
					int start = (int)param * blockSize + 1;

					for (int i = start; i < start + blockSize; i++)
					{
						for (int j = 1; j < Width - 1; j++)
						{
							var data = (Pixels[i - 1, j] + Pixels[i + 1, j] + Pixels[i, j - 1] + Pixels[i, j + 1]) * 0.1f + Pixels[i, j] * 0.6f;
							tempPixels[i, j] = data;
						}
					}
				}, k);
				tasks[k] = t;
				t.Start();
			}

			Task.WaitAll(tasks);

			Pixels = tempPixels;
		}

		public void ConvolutionAsyncPool()
		{
			newPixels = new float[Height, Width];
			int numberOfThreads = 8;
			AutoResetEvent[] autoResetEvents = new AutoResetEvent[numberOfThreads];

			for (int i = 0; i < numberOfThreads; i++)
			{
				autoResetEvents[i] = new AutoResetEvent(false);
				ThreadPool.QueueUserWorkItem(Calculate, new Object[] { i, numberOfThreads, autoResetEvents[i] });
			}

			WaitHandle.WaitAll(autoResetEvents);

			Pixels = newPixels;
		}

		public void Calculate(Object obj)
		{
			Object[] temp = (Object[])obj;
			int param = (int)temp[0];
			int numberOfThreads = (int)temp[1];

			int blockSize = 1024 / numberOfThreads;
			int start = param * blockSize + 1;

			for (int i = start; i < start + blockSize; i++)
			{
				for (int j = 1; j < Width - 1; j++)
				{
					var data = (Pixels[i - 1, j] + Pixels[i + 1, j] + Pixels[i, j - 1] + Pixels[i, j + 1]) * 0.1f + Pixels[i, j] * 0.6f;
					newPixels[i, j] = data;
				}
			}

			((AutoResetEvent)temp[2]).Set();
		}

	}
}
