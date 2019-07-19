using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace Custom.UI
{
    [Serializable]
    public struct TextBlock : IEquatable<TextBlock>
    {
        [FormerlySerializedAs("normalText")]
        [SerializeField]
        private string m_NormalText;

        [FormerlySerializedAs("highlightedText")]
        [FormerlySerializedAs("m_SelectedText")]
        [SerializeField]
        private string m_HighlightedText;

        [FormerlySerializedAs("pressedText")]
        [SerializeField]
        private string m_PressedText;

        [FormerlySerializedAs("disabledText")]
        [SerializeField]
        private string m_DisabledText;

        public string normalText { get { return m_NormalText; } set { m_NormalText = value; } }
        public string highlightedText { get { return m_HighlightedText; } set { m_HighlightedText = value; } }
        public string pressedText { get { return m_PressedText; } set { m_PressedText = value; } }
        public string disabledText { get { return m_DisabledText; } set { m_DisabledText = value; } }

        public static TextBlock defaultTextBlock
        {
            get
            {
                var c = new TextBlock
                {
                    m_NormalText = "",
                    m_HighlightedText = "",
                    m_PressedText = "",
                    m_DisabledText = "",
                };
                return c;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TextBlock))
                return false;

            return Equals((TextBlock)obj);
        }

        public bool Equals(TextBlock other)
        {
            return normalText == other.normalText &&
                highlightedText == other.highlightedText &&
                pressedText == other.pressedText &&
                disabledText == other.disabledText;
        }

        public static bool operator ==(TextBlock point1, TextBlock point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(TextBlock point1, TextBlock point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
