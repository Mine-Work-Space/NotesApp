using System.Text.RegularExpressions;

namespace NotesApp.Editors
{
	//
	// Summary:
	//     Provides static method for words selection
	public static class TextEditor
	{
		/// <summary>
		/// Finds a keyword in a text and highlights it
		/// </summary>
		/// <param name="cssClass">
		///		value of background-color parameter. Default = dark yellow in hex
		/// </param>
		/// <param name="keyword">
		///		word or phrase to look for
		/// </param>
		/// <param name="text">
		///		string in which the required word is searched
		/// </param>
		///	<returns>The same text in string with highlighted keywords</returns>
		public static string HighlightKeyWords(string text, string keyword, string cssClass = "#99781a")
		{
			if (text == String.Empty || keyword == String.Empty || cssClass == String.Empty)
				return text;
			return Regex.Replace(text,
				keyword,
				string.Format("<span style=\"background-color:{0}\">{1}</span>", cssClass, "$0"),
				RegexOptions.IgnoreCase);
		}
	}
}
