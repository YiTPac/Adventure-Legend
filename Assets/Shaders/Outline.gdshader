shader_type canvas_item;

uniform float outlineWidth = 1.0;
uniform vec4 outlineColor : source_color = vec4(0, 0, 0, 1);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 uv = UV;
	vec2 uvUp = uv + vec2(0, TEXTURE_PIXEL_SIZE.y) * outlineWidth;
	vec2 uvDown = uv + vec2(0, -TEXTURE_PIXEL_SIZE.y) * outlineWidth;
	vec2 uvLeft = uv + vec2(TEXTURE_PIXEL_SIZE.x, 0) * outlineWidth;
	vec2 uvRight = uv + vec2(-TEXTURE_PIXEL_SIZE.x, 0) * outlineWidth;
	
	vec4 colorUp = texture(TEXTURE, uvUp);
	vec4 colorDown = texture(TEXTURE, uvDown);
	vec4 colorLeft = texture(TEXTURE, uvLeft);
	vec4 colorRight = texture(TEXTURE, uvRight);
	
	vec4 outline = (colorUp + colorDown + colorLeft + colorRight);
	outline.rgb = outlineColor.rgb;
	
	vec4 originalColor = texture(TEXTURE, UV);
	
	COLOR = mix(outline, originalColor, originalColor.a);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
