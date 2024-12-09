export const formatDate = (dateString) => {
  if (!dateString) return '';

  console.log("dateString: ", dateString)
  
  const date = new Date(dateString);
  const now = new Date();
  const diffTime = Math.abs(now - date);
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  
  if (diffDays === 0) {
    return 'Publicado hoy';
  } else if (diffDays === 1) {
    return 'Publicado ayer';
  } else {
    return `Publicado hace ${diffDays} días`;
  }
};

export const formatSalaryRange = (share, currency = 'USD') => {
  
  const formatter = new Intl.NumberFormat('es-AR', {
    style: 'currency',
    currency,
    minimumFractionDigits: 0,
    maximumFractionDigits: 0
  });

  return `${formatter.format(share)}`;
};