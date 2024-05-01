export const checkNullish = (test: unknown) => {
  if (!!test && test !== "undefined" && test !== "null") return test;
  return undefined;
};
