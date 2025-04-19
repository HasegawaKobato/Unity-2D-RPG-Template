using UnityEngine;


namespace KBT
{
    class SplitTexture
    {
        public Texture2D leftTop;
        public Texture2D centerTop;
        public Texture2D rightTop;
        public Texture2D leftCenter;
        public Texture2D center;
        public Texture2D rightCenter;
        public Texture2D leftBottom;
        public Texture2D centerBottom;
        public Texture2D rightBottom;
    }

    [RequireComponent(typeof(SpriteRenderer)), ExecuteInEditMode]
    public class GenerateTilemap : MonoBehaviour
    {
        [SerializeField] private Sprite sourceTile;
        [SerializeField] private bool update;

        private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (update)
            {
                if (sourceTile != null)
                {
                    update = false;
                    updateTexture();
                }
            }
        }

        private void updateTexture()
        {
            SplitTexture splitTexture = getSourceSplitTexture(sourceTile);

            Texture2D lb = new Texture2D(64, 32);
            lb.SetPixels(0, 0, 64, 32, splitTexture.leftBottom.GetPixels());
        }

        private SplitTexture getSourceSplitTexture(Sprite sourceSprite)
        {

            Rect sourceRect = sourceSprite.textureRect;
            int sourceX = Mathf.FloorToInt(sourceRect.x);
            int sourceY = Mathf.FloorToInt(sourceRect.y);
            int sourceBorderL = Mathf.FloorToInt(sourceSprite.border.x);
            int sourceBorderB = Mathf.FloorToInt(sourceSprite.border.y);
            int sourceBorderR = Mathf.FloorToInt(sourceSprite.border.z);
            int sourceBorderT = Mathf.FloorToInt(sourceSprite.border.w);
            int sourceCenterW = Mathf.FloorToInt(sourceSprite.textureRect.width) - sourceBorderL - sourceBorderR;
            int sourceCenterH = Mathf.FloorToInt(sourceSprite.textureRect.height) - sourceBorderB - sourceBorderT;

            // ================= 左下 =================
            Texture2D lb = new Texture2D(sourceBorderL, sourceBorderB);
            Color[] lbColors = sourceSprite.texture.GetPixels(sourceX, sourceY, sourceBorderL, sourceBorderB);
            lb.SetPixels(0, 0, sourceBorderL, sourceBorderB, lbColors);
            lb.Apply();
            // Sprite lbSprite = Sprite.Create(lb, new Rect(0, 0, sourceBorderL, sourceBorderB), Vector2.one * 0.5f);
            lb.wrapMode = TextureWrapMode.Clamp;
            lb.filterMode = FilterMode.Point;

            // ================= 左中 =================
            Texture2D lc = new Texture2D(sourceBorderL, sourceCenterH);
            Color[] lcColors = sourceSprite.texture.GetPixels(sourceX, sourceY + sourceBorderB, sourceBorderL, sourceCenterH);
            lc.SetPixels(0, 0, sourceBorderL, sourceCenterH, lcColors);
            lc.Apply();
            // Sprite lcSprite = Sprite.Create(lc, new Rect(0, 0, sourceBorderL, sourceCenterH), Vector2.one * 0.5f);
            lc.wrapMode = TextureWrapMode.Clamp;
            lc.filterMode = FilterMode.Point;

            // ================= 左上 =================
            Texture2D lt = new Texture2D(sourceBorderL, sourceBorderT);
            Color[] ltColors = sourceSprite.texture.GetPixels(sourceX, sourceY + sourceBorderB + sourceCenterH, sourceBorderL, sourceBorderT);
            lt.SetPixels(0, 0, sourceBorderL, sourceBorderT, ltColors);
            lt.Apply();
            // Sprite ltSprite = Sprite.Create(lt, new Rect(0, 0, sourceBorderL, sourceBorderT), Vector2.one * 0.5f);
            lt.wrapMode = TextureWrapMode.Clamp;
            lt.filterMode = FilterMode.Point;

            // ================= 中上 =================
            Texture2D ct = new Texture2D(sourceCenterW, sourceBorderT);
            Color[] ctColors = sourceSprite.texture.GetPixels(sourceX + sourceBorderL, sourceY + sourceBorderB + sourceCenterH, sourceCenterW, sourceBorderT);
            ct.SetPixels(0, 0, sourceCenterW, sourceBorderT, ctColors);
            ct.Apply();
            // Sprite ctSprite = Sprite.Create(ct, new Rect(0, 0, sourceCenterW, sourceBorderT), Vector2.one * 0.5f);
            ct.wrapMode = TextureWrapMode.Clamp;
            ct.filterMode = FilterMode.Point;

            // ================= 正中 =================
            Texture2D center = new Texture2D(sourceCenterW, sourceCenterH);
            Color[] centerColors = sourceSprite.texture.GetPixels(sourceX + sourceBorderL, sourceY + sourceBorderB, sourceCenterW, sourceCenterH);
            center.SetPixels(0, 0, sourceCenterW, sourceCenterH, centerColors);
            center.Apply();
            // Sprite centerSprite = Sprite.Create(center, new Rect(0, 0, sourceCenterW, sourceCenterH), Vector2.one * 0.5f);
            center.wrapMode = TextureWrapMode.Clamp;
            center.filterMode = FilterMode.Point;

            // ================= 中下 =================
            Texture2D cb = new Texture2D(sourceCenterW, sourceBorderB);
            Color[] cbColors = sourceSprite.texture.GetPixels(sourceX + sourceBorderL, sourceY, sourceCenterW, sourceBorderB);
            cb.SetPixels(0, 0, sourceCenterW, sourceBorderB, cbColors);
            cb.Apply();
            // Sprite cbSprite = Sprite.Create(cb, new Rect(0, 0, sourceCenterW, sourceBorderB), Vector2.one * 0.5f);
            cb.wrapMode = TextureWrapMode.Clamp;
            cb.filterMode = FilterMode.Point;

            // ================= 右上 =================
            Texture2D rt = new Texture2D(sourceBorderR, sourceBorderT);
            Color[] rtColors = sourceSprite.texture.GetPixels(sourceX + sourceBorderL + sourceCenterW, sourceY + sourceBorderB + sourceCenterH, sourceBorderR, sourceBorderT);
            rt.SetPixels(0, 0, sourceBorderR, sourceBorderT, rtColors);
            rt.Apply();
            // Sprite rtSprite = Sprite.Create(rt, new Rect(0, 0, sourceBorderR, sourceBorderT), Vector2.one * 0.5f);
            rt.wrapMode = TextureWrapMode.Clamp;
            rt.filterMode = FilterMode.Point;

            // ================= 右中 =================
            Texture2D rc = new Texture2D(sourceBorderR, sourceCenterH);
            Color[] rcColors = sourceSprite.texture.GetPixels(sourceX + sourceBorderL + sourceCenterW, sourceY + sourceBorderB, sourceBorderR, sourceCenterH);
            rc.SetPixels(0, 0, sourceBorderR, sourceCenterH, rcColors);
            rc.Apply();
            // Sprite rcSprite = Sprite.Create(rc, new Rect(0, 0, sourceBorderR, sourceCenterH), Vector2.one * 0.5f);
            rc.wrapMode = TextureWrapMode.Clamp;
            rc.filterMode = FilterMode.Point;

            // ================= 右下 =================
            Texture2D rb = new Texture2D(sourceBorderR, sourceBorderB);
            Color[] rbColors = sourceSprite.texture.GetPixels(sourceX + sourceBorderL + sourceCenterW, sourceY, sourceBorderR, sourceBorderB);
            rb.SetPixels(0, 0, sourceBorderR, sourceBorderB, rbColors);
            rb.Apply();
            // Sprite rbSprite = Sprite.Create(rb, new Rect(0, 0, sourceBorderR, sourceBorderB), Vector2.one * 0.5f);
            rb.wrapMode = TextureWrapMode.Clamp;
            rb.filterMode = FilterMode.Point;

            SplitTexture splitTexture = new SplitTexture()
            {
                leftTop = lt,
                centerTop = ct,
                rightTop = rt,
                leftCenter = lc,
                center = center,
                rightCenter = rc,
                leftBottom = lb,
                centerBottom = cb,
                rightBottom = rb,
            };


            return splitTexture;

        }

    }

}
