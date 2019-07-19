using System;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.UI
{
    public static class FontUpdateTrackerUI
    {
        static Dictionary<Font, HashSet<TextUI>> m_Tracked = new Dictionary<Font, HashSet<TextUI>>();

        public static void TrackText(TextUI t)
        {
            if (t.font == null)
                return;

            HashSet<TextUI> exists;
            m_Tracked.TryGetValue(t.font, out exists);
            if (exists == null)
            {
                // The textureRebuilt event is global for all fonts, so we add our delegate the first time we register *any* Text
                if (m_Tracked.Count == 0)
                    Font.textureRebuilt += RebuildForFont;

                exists = new HashSet<TextUI>();
                m_Tracked.Add(t.font, exists);
            }

            if (!exists.Contains(t))
                exists.Add(t);
        }

        private static void RebuildForFont(Font f)
        {
            HashSet<TextUI> texts;
            m_Tracked.TryGetValue(f, out texts);

            if (texts == null)
                return;

            foreach (var text in texts)
                text.FontTextureChanged();
        }

        public static void UntrackText(TextUI t)
        {
            if (t.font == null)
                return;

            HashSet<TextUI> texts;
            m_Tracked.TryGetValue(t.font, out texts);

            if (texts == null)
                return;

            texts.Remove(t);

            if (texts.Count == 0)
            {
                m_Tracked.Remove(t.font);

                // There is a global textureRebuilt event for all fonts, so once the last Text reference goes away, remove our delegate
                if (m_Tracked.Count == 0)
                    Font.textureRebuilt -= RebuildForFont;
            }
        }
    }
}
