using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Innovoft.IO
{
	public sealed class CopyStream : Stream
	{
		#region Fields
		private Stream? reader;
		private Action? readerDispose;
		private Stream? writer;
		private Action? writerDispose;
		#endregion //Fields

		#region Constructors
		public CopyStream()
		{
		}

		public CopyStream(Stream reader, Stream writer)
		{
			this.reader = reader;
			this.readerDispose = reader.Dispose;
			this.writer = writer;
			this.writerDispose = writer.Dispose;
		}

		public CopyStream(Stream reader, bool readerDispose, Stream writer, bool writerDispose)
		{
			this.reader = reader;
			if (readerDispose)
			{
				this.readerDispose = reader.Dispose;
			}
			else
			{
				this.readerDispose = null;
			}
			this.writer = writer;
			if (writerDispose)
			{
				this.writerDispose = writer.Dispose;
			}
			else
			{
				this.writerDispose = null;
			}
		}

		public CopyStream(Stream reader, Action? readerDispose, Stream writer, Action? writerDispose)
		{
			this.reader = reader;
			this.readerDispose = readerDispose;
			this.writer = writer;
			this.writerDispose = writerDispose;
		}
		#endregion //Constructors

		#region Properties
		public override bool CanRead => true;
		public override bool CanWrite => false;
		public override bool CanSeek => false;
		public override long Length => throw new NotImplementedException();
		public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		#endregion //Properties

		#region Methods
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				base.Dispose(disposing: false);
				return;
			}

			reader = null;
			if (readerDispose != null)
			{
				readerDispose();
				readerDispose = null;
			}
			writer = null;
			if (writerDispose != null)
			{
				writerDispose();
				writerDispose = null;
			}

			base.Dispose(disposing: true);
		}

		public void SetReader(Stream reader)
		{
			this.reader = reader;
			this.readerDispose = reader.Dispose;
		}

		public void SetReader(Stream reader, bool dispose)
		{
			this.reader = reader;
			if (dispose)
			{
				this.readerDispose = reader.Dispose;
			}
			else
			{
				this.readerDispose = null;
			}
		}

		public void SetReader(Stream reader, Action? dispose)
		{
			this.reader = reader;
			this.readerDispose = dispose;
		}

		public void SetWriter(Stream writer)
		{
			this.writer = writer;
			this.writerDispose = writer.Dispose;
		}

		public void SetWriter(Stream writer, bool dispose)
		{
			this.writer = writer;
			if (dispose)
			{
				this.writerDispose = writer.Dispose;
			}
			else
			{
				this.writerDispose = null;
			}
		}

		public void SetWriter(Stream writer, Action? disopse)
		{
			this.writer = writer;
			this.writerDispose = disopse;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Flush()
		{
			writer.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var read = reader.Read(buffer, offset, count);
			writer.Write(buffer, offset, read);
			return read;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			var read = await reader.ReadAsync(buffer, offset, count, cancellationToken);
			await writer.WriteAsync(buffer, offset, read, cancellationToken);
			return read;
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
		#endregion //Methods
	}
}
