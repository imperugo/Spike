using System.Linq;
using System.Text.RegularExpressions;

namespace imperugo.linkflood.scheduler.Helpers {
	internal static class ParserHelper {
		public static string Linkify(string searchText) {
			Regex regx = new Regex(@"\b(((\S+)?)(@|mailto\:|(news|(ht|f)tp(s?))\://)\S+)\b", RegexOptions.IgnoreCase);
			searchText = searchText.Replace("&nbsp;", " ");
			MatchCollection matches = regx.Matches(searchText);

			foreach (Match match in matches) {
				
				//Added exception for asp.net word
				if(match.Value.ToLowerInvariant() == "asp.net")
					continue;

				if (match.Value.StartsWith("http")) {
					searchText = searchText.Replace(match.Value, "</strong><a href='" + match.Value + "'>" + match.Value + "</a><strong>");
				}
			}

			return searchText;
		}

		public static string LinkifyMentions(string text) {
			var matches = Regex.Matches(text, @"@(?<tag>\w{1,100})").Cast<Match>().Select(x => x.Value.Trim()).ToList();

			foreach (var match in matches)
			{
				if (string.IsNullOrEmpty(match))
					continue;

				string value = string.Format("</strong><a href=\"http://twitter.com/{0}\">{1}</a><strong>", match.Replace("@", string.Empty), match);
				text = text.Replace(match, value);
			}

			return text;
		}

		public static string LinkifyHashtags(string text) {
			var matches = Regex.Matches(text, @"#(?<tag>\w{1,100})").Cast<Match>().Select(x => x.Value.Trim()).ToList();

			foreach (var match in matches)
			{
				if (string.IsNullOrEmpty(match))
					continue;

				string value = string.Format("</strong><a href=\"http://twitter.com/search/%23{0}\">{1}</a><strong>", match.Replace("#", string.Empty), match);
				text = text.Replace(match, value);
			}

			return text;
		}
	}
}