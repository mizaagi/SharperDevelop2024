﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Siegfried Pammer" email="siegfriedpammer@gmail.com"/>
//     <version>$Revision$</version>
// </file>
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.SharpDevelop.Editor;

namespace ICSharpCode.AvalonEdit.AddIn
{
	public class BracketHighlightRenderer : IBackgroundRenderer
	{
		BracketSearchResult result;
		Pen borderPen;
		TextView textView;
		
		public void SetHighlight(BracketSearchResult result)
		{
			this.result = result;
			var layer = textView.GetKnownLayer(Layer);
			if (layer != null)
				layer.InvalidateVisual();
		}
		
		public BracketHighlightRenderer(TextView textView)
		{
			if (textView == null)
				throw new ArgumentNullException("textView");
			
			this.borderPen = new Pen(Brushes.Blue, 1);
			this.borderPen.Freeze();
			this.textView = textView;
			
			//this.textView.BackgroundRenderers.Add(this);
		}
		
		public KnownLayer Layer {
			get {
				return KnownLayer.Selection;
			}
		}
		
		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			if (this.result == null)
				return;
			
			BackgroundGeometryBuilder builder = new BackgroundGeometryBuilder();
			
			builder.CornerRadius = 1;
			
			builder.AddSegment(textView, new TextSegment() { StartOffset = result.OpeningBracketOffset, Length = result.OpeningBracketLength });
			builder.AddSegment(textView, new TextSegment() { StartOffset = result.ClosingBracketOffset, Length = result.ClosingBracketLength });
			
			PathGeometry geometry = builder.CreateGeometry();
			
			if (geometry != null) {
				geometry.Freeze();
				drawingContext.DrawGeometry(Brushes.LightBlue, borderPen, geometry);
			}
		}
	}
}