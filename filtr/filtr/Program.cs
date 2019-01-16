using System;
using System.Diagnostics;

namespace filtr
{
	class Program
	{
		static void Main(string[] args)
		{
			ImagePGM image = new ImagePGM();
			image.Generate();
			ImageSaver saver = new ImageSaver();
			saver.save("input.pgm", image);
			ImagePGMasync image2 = new ImagePGMasync();
			ImagePGMasync image3 = new ImagePGMasync();
			image2.Generate();
			image3.Generate();
			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();
			for (int i = 0; i < 200; i++)
			{
				image.Convolution();
			}
			stopwatch.Stop();

			Console.WriteLine("Synchronicznie: {0}s", stopwatch.Elapsed.TotalSeconds);
			saver.save("sync.pgm", image);

			stopwatch.Restart();
			for (int i = 0; i < 200; i++)
			{
				image2.ConvolutionAsyncTask();
			}
			stopwatch.Stop();

			Console.WriteLine("Asynchronicznie (Task): {0}s", stopwatch.Elapsed.TotalSeconds);
			ImageSaverPGMasync saverPGMasync = new ImageSaverPGMasync();
			saverPGMasync.save("async.pgm", image2);

			stopwatch.Restart();
			for (int i = 0; i < 200; i++)
			{
				image3.ConvolutionAsyncPool();
			}
			stopwatch.Stop();

			Console.WriteLine("Asynchronicznie (ThreadPool): {0}s", stopwatch.Elapsed.TotalSeconds);
			saverPGMasync.save("asyncc2.pgm", image3);
		}
	}
}
