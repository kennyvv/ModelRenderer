using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ModelRenderer.STL;

namespace ModelRenderer
{
	public static class Extensions
	{
		public static Texture2D FromImage(this string file, GraphicsDevice gd)
		{
			using (FileStream fs = new FileStream(file, FileMode.Open))
			{
				return Texture2D.FromStream(gd, fs);
			}
		}

		/// <summary>Inverts the <see cref="Normal"/> within the <paramref name="facets"/> enumerable.</summary>
		/// <param name="facets">The facets to invert.</param>
		public static void Invert(this IEnumerable<Facet> facets)
		{
		//	facets.ForEach(f => f.Normal.Invert());
		}

		/// <summary>Iterates the provided enumerable, applying the provided action to each element.</summary>
		/// <param name="items">The items upon which to apply the action.</param>
		/// <param name="action">The action to apply to each item.</param>
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					action(item);
				}
			}
		}

		/// <summary>Iterates the provided enumerable, applying the provided action to each element.</summary>
		/// <param name="items">The items upon which to apply the action.</param>
		/// <param name="predicate">The action to apply to each item.</param>
		public static bool All<T>(this IEnumerable<T> items, Func<int, T, bool> predicate)
		{
			if (items != null)
			{
				var index = 0;

				foreach (var item in items)
				{
					if (!predicate(index, item))
					{
						return false;
					}

					index++;
				}
			}

			return true;
		}

		/// <summary>Checks if the provided value is null or empty.</summary>
		/// <param name="value">The value to check.</param>
		/// <returns>True if the provided value is null or empty.</returns>
		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>Interpolates the provided formatted string with the provided args using the default culture.</summary>
		/// <param name="format">The formatted string.</param>
		/// <param name="args">The values to use for interpolation.</param>
		public static string Interpolate(this string format, params object[] args)
		{
			return format.Interpolate(CultureInfo.InvariantCulture, args);
		}

		/// <summary>Interpolates the provided formatted string with the provided args.</summary>
		/// <param name="format">The formatted string.</param>
		/// <param name="culture">The culture info to use.</param>
		/// <param name="args">The values to use for interpolation.</param>
		public static string Interpolate(this string format, CultureInfo culture, params object[] args)
		{
			if (format != null)
			{
				return string.Format(culture, format, args);
			}

			return null;
		}
	}
}
