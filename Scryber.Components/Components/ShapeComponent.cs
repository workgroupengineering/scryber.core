﻿/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Native;

namespace Scryber.Components
{
    public abstract class ShapeComponent : VisualComponent, IPDFGraphicPathComponent
    {

        public ShapeComponent(PDFObjectType type) : base(type) { }

        private PDFGraphicsPath _path;

        protected PDFGraphicsPath Path
        {
            get { return _path; }
            set { _path = value; }
        }

        PDFGraphicsPath IPDFGraphicPathComponent.Path
        {
            get { return this.Path; }
            set { this.Path = value; }
        }


        protected abstract PDFGraphicsPath CreatePath(PDFSize available, PDFStyle fullstyle);

        PDFGraphicsPath IPDFGraphicPathComponent.CreatePath(PDFSize available, PDFStyle fullstyle)
        {
            return this.CreatePath(available, fullstyle);
        }

        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFStyle fullstyle = context.FullStyle;
            if (null == fullstyle)
                throw new ArgumentNullException("context.FullStyle");

            PDFGraphics graphics = context.Graphics;
            if (null == graphics)
                throw new ArgumentNullException("context.Graphics");

            if (null != this.Path)
            {
                PDFBrush brush = fullstyle.CreateFillBrush();
                PDFPen pen = fullstyle.CreateStrokePen();

                if (null != pen && null != brush)
                    graphics.FillAndStrokePath(brush, pen, context.Offset, this.Path);
                
                else if (null != brush)
                    graphics.FillPath(brush, context.Offset, this.Path);
                
                else if (null != pen)
                    graphics.DrawPath(pen, context.Offset, this.Path);

               
            }
            return null;
        }

    }
}