# Rediseño UX/UI de la Home

Rama: `feat/rediseno-home-ux` · Referencia: prestashop.es (secciones planas, bien delimitadas y minimalistas).

## Diagnóstico de la home actual

| Problema | Dónde | Impacto |
|---|---|---|
| Degradados y "glows" radiales en el fondo | Hero, slider, paneles, bento | Ruido visual, contraste irregular, aspecto recargado |
| H1 = nombre de marca ("FINANCEHORIZON") | Hero | El H1 no comunica la propuesta de valor; marca duplicada con el navbar |
| Carrusel autoplay como contenido principal | Hero | Solo se ve la 1.ª diapositiva; el resto pasa desapercibido y distrae |
| Contenido repetido | Hero + "Últimas entradas" | Los mismos artículos aparecen dos veces |
| Paneles de colores saturados (amarillo / azul) | Últimas entradas | Compiten con el contenido; jerarquía confusa |
| Texto persuasivo gigante en dos colores | Sección de estadísticas | Bloque sin acción y poco legible |
| Bento oscuro con texto sobre foto + scrims | Categorías | Legibilidad dependiente de la imagen; 4 variantes de tarjeta casi iguales (código duplicado) |
| Botones pill de varios estilos | Toda la home | Falta un sistema de botones coherente |

## Principios aplicados

1. **Jerarquía tipográfica clara**: eyebrow (12px, mayúsculas) → H1 display (mayúsculas, 900) → H2 de sección (mayúsculas, 800) → H3 de tarjeta (700) → cuerpo (16–18px).
2. **Sin degradados**: solo colores sólidos de la marca (navy `#00296B`, amarillo `#FDC500`) y tintes planos (`#E8EFF8`, `#FFF6D6`, `#F5F6F8`).
3. **Una sección = un objetivo = un fondo**. Cada bloque tiene título, contenido y, como mucho, una acción.
4. **Sistema de botones único**: rectangular, mayúsculas, flecha ▶; variantes sólida, invertida y contorno.
5. **Ritmo vertical consistente** (escala de 8px; padding de sección `clamp(4rem, 8vw, 7.5rem)`).
6. **Texto fuera de las fotos**: las imágenes ilustran, el texto va en superficie sólida (contraste AA garantizado).
7. **Sin contenido duplicado** y sin autoplay.
8. Accesibilidad: foco visible, `prefers-reduced-motion`, objetivos táctiles ≥ 44px, `alt`, landmarks y `aria-labelledby` por sección.

## Nueva estructura de la home

| # | Sección | Fondo | Contenido | Acción |
|---|---|---|---|---|
| 1 | **Hero** | Navy sólido | Eyebrow con la marca, H1 de propuesta de valor a la izquierda; texto breve + buscador a la derecha | Buscar |
| 2 | **Destacados** | Navy → blanco (corte recto) | 1 artículo principal grande + lista de 4 destacados | Leer artículo |
| 3 | **Franja de categorías** | Blanco | Accesos rápidos a las categorías principales (como la franja de logos de la referencia) | Ir a categoría |
| 4 | **Últimas entradas** | Blanco | Rejilla de 4 tarjetas (imagen + categoría + título + fecha), sin repetir destacados | Ver todas |
| 5 | **Lo más leído** | Tinte azul plano | Título grande a la izquierda + ranking numerado 01–05 a la derecha | — |
| 6 | **Cifras del archivo** | Blanco | Título + 3 métricas separadas por divisores verticales, cada una con su botón de contorno | Publicaciones / Categorías |
| 7 | **Categorías principales** | Gris superficie | Tarjetas uniformes: imagen arriba, nombre, descripción y subcategorías | Ver categoría |
| 8 | **CTA final** | Amarillo sólido | "¿Buscas algo concreto?" + buscador | Buscar |

## Cambios técnicos

- `Views/Home/Index.cshtml`: maquetación reescrita por secciones con BEM `hp-*`.
- `Views/Home/Partials/_HomeCategoryCard.cshtml`: tarjeta de categoría única (sustituye 4 bloques duplicados).
- `Styles/components/home-landing.css`: estilos nuevos de la home (sin degradados); se importa desde `tailwind.css`.
- `Models/Home/HomeViewModels.cs`: H1 con propuesta de valor, eyebrow con la marca, "Últimas entradas" excluye los destacados, lista ordenada de tarjetas de categoría y categorías principales para la franja.
- Se retira el carrusel (`hero-dynamic.js`) de la home; el resto de páginas no cambia.
- Test nuevo: las últimas entradas no repiten artículos del bloque de destacados.

## Fase 2: resto de vistas públicas

| Vista | Cambio |
|---|---|
| Categoría, Publicaciones, Info | Cabecera `page-hero` en navy sólido (sin brillo ni borde amarillo), título display en mayúsculas, subcategorías como etiquetas rectangulares |
| Publicaciones | Buscador y contador en la cabecera (antes solo se podía buscar desde la home) |
| Listados | `_PostCardMosaic` pasa a la tarjeta `hp-card` (imagen arriba, texto en superficie sólida); rejilla `hp-grid--3` |
| Estados vacíos y paginación | Bloque plano `hp-empty` con botón del sistema; paginación rectangular sin sombras |
| Artículo y parciales | Sin degradados, brillos ni sombras; radios unificados (8px contenedores, 2px botones, etiquetas y campos) |
| Contenido del artículo | Divisor `hr` sólido y `<mark>` con subrayado sólido (`box-shadow`) en lugar de degradado |
| Navbar | Cabecera del modal de WhatsApp en color sólido |
| HTML | Las vistas públicas ya no anidan un segundo `<main>` dentro del layout |

- Los tokens `--hp-*` son globales (`:root`) y `home.css` queda reducido a contenedor, foco y reveal.
- Se eliminan `hero-dynamic.js` y `_HeroDynamic.cshtml` (carrusel sin uso).

## Fuera de alcance

- Panel de administración (`editor.css` y vistas de `Areas/Admin`).
- Navbar y footer: solo se retocó el modal de WhatsApp; el botón "Únete" mantiene su estilo.
