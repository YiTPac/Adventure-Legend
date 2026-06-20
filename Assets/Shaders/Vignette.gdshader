shader_type canvas_item;

// 提醒：需要在你的 ColorRect 节点的 CanvasItem -> Material 属性里，
// 将 Blend Mode（混合模式）保持为 Mix（默认），或者检查下方的说明。
uniform sampler2D screen_texture : hint_screen_texture, filter_linear_mipmap;

uniform float alpha = 1.0;
uniform float inner_radius = 0.0;
uniform float outer_radius = 1.0;

void fragment() {
    // 1. 修正 UV 获取方式：
    // 原来的 UV.r 和 UV.g 是当前 ColorRect 自身的坐标。
    // 在后处理中，使用 SCREEN_UV 抓取对应的屏幕位置最为精准。
    float x = abs(UV.x - 0.5) * 2.0;
    float y = abs(UV.y - 0.5) * 2.0;
    
    // 2. 保留你原有的圆形/椭圆暗角数学计算公式
    float q = 1.0 - (1.0 - sqrt(x*x + y*y) / outer_radius) / (1.0 - inner_radius);
    
    // 3. 限制 q 的范围在 0.0 到 1.0 之间，防止极端数值导致溢出
    q = clamp(q, 0.0, 1.0);
    
    // 4. 获取屏幕原本的 HDR 颜色（包含大于 1.0 的高亮信息）
    vec3 screen_color = texture(screen_texture, UV).rgb;
    
    // 5. 核心修改：通过乘法让边缘变暗
    // 当在中心时，q 接近 0，(1.0 - q * alpha) 接近 1.0 -> 保持原样
    // 当在边缘时，q 接近 1，(1.0 - q * alpha) 变小 -> 完美等比例压暗 HDR 颜色
    vec3 final_color = screen_color * (1.0 - q * alpha);
    
    // 输出最终颜色，不依赖透明度混合，Alpha 直接给 1.0 完美覆盖
    COLOR = vec4(final_color, 1.0);
}
