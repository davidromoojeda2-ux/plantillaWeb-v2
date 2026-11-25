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

## Fase 3: navbar y footer

| Pieza | Cambio |
|---|---|
| Navbar | Fondo blanco sólido con línea inferior de 1px (sin blur ni sombra al hacer scroll); ancho alineado con `.container`; enlaces en mayúsculas 800 con subrayado amarillo recto pegado a la línea |
| Botón "Únete" | Formato de `.hp-btn` (rectangular, 2px, mayúsculas, sin sombra ni escalado) conservando el verde WhatsApp |
| Mega-menú y menú móvil | Franja amarilla plana sin sombra; encabezados con borde navy, marcadores cuadrados, sin desplazamientos al hover |
| Modal de WhatsApp | Contenedor de 8px sin sombra ni blur; controles y elementos de 2px; tipografía del sistema |
| Destacado principal (home) | Dinámico (imagen del destacado más reciente, sin carrusel). Imagen completa sin recorte (`object-fit: contain`) junto a un panel navy con categoría, título, fecha y "Leer artículo"; "También destacado" pasa a una fila de 4 debajo |
| Franja de categorías | Tarjetas más altas y separadas en escritorio; en móvil se apilan en vertical (2 columnas en tablet). La página de categoría ya no la usa |
| Hero de categoría y subcategoría | Título, descripción y contador centrados arriba; debajo, el artículo más reciente con imagen mediante el componente común `Partials/_LeadArticle` (imagen completa + panel con el texto a la derecha), el mismo que los destacados de la home. `CategoryPageViewModel.HeroArticle` sustituye a `HeroImageUrl`/`HeroImageSrcSet`. El artículo destacado no se repite en la rejilla (`GridArticles` excluye `HeroArticleId`; `Articles` sigue completo para los datos estructurados) |
| Subcategorías | Menú numerado (01, 02…) bajo el hero con filetes navy, la actual resaltada en navy y "Ver todo en …" en subcategorías; solo aparece si existen |
| Artículo | Cabecera navy (`page-hero` + `article-hero__*`): migas, anterior/siguiente, categoría, título en mayúsculas, resumen, autor y datos. Barra de compartir solo con iconos cuadrados (`share-bar`) al pie de la cabecera, antes de la portada. Cabecera centrada. Portada completa sin recorte, sobre la frontera navy / blanco con corte recto (franja navy sólida detrás de la parte alta, como los destacados de la home; sin degradado). Contenido y comentarios sobre blanco, sin tarjetas |
| Índice del artículo | "En esta página" sin caja navy: filete navy arriba, título en mayúsculas, contador, enlaces con filete lateral y la sección activa en tinte azul con filete amarillo (`.toc*`, clases asignadas por `article.js`). En móvil, plegable entre filetes navy |
| Tarjeta de artículo única | `Partials/_PostCard` → `_PostCardMosaic` (`hp-card`) es el único componente de tarjeta: home ("Últimas entradas", antes duplicada en línea), publicaciones, categorías y "También te puede interesar". `PostCardViewModel` añade `ShowExcerpt` y `ImageSizes` (`SizesFourColumns` / `SizesThreeColumns`). `reveal.js` se carga una vez en el layout en lugar de por página |
| También te puede interesar | Cabecera de sección `hp-h2` y rejilla `hp-grid--3` con la tarjeta común, sin resumen |
| Políticas (Info) y error | Menú "Información legal" con el mismo estilo `.toc` que el índice del artículo (página actual resaltada); se eliminan `_MenuBrand` y `_CollapsibleMenuHeader`, ya sin uso. Contenido sin tarjeta con borde; la página de error pasa a cabecera navy con botón del sistema |
| Pantallas muy grandes (≥1920px) | Escala proporcional solo en vistas públicas (`html.site-public`): tamaño raíz `clamp(100%, 0.8333vw, 175%)` (16px a 1920, ~21px a 2560, 28px a 3840) y contenedor (`100rem`) y navbar en `rem`. Todo crece en bloque como un zoom; el admin no cambia |
| Footer | Navy sólido con BEM `site-footer__*` (`Styles/components/footer.css`), encabezados amarillos con filete, iconos sociales cuadrados y texto de marca de finanzas |
