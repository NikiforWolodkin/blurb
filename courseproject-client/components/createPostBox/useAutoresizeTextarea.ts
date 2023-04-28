import { useEffect } from "react";

/** Updates the height of a textarea when the text changes and goes on a new line. */
const useAutoresizeTextarea = (
  textAreaRef: HTMLTextAreaElement | null,
  value: string
) => {
  useEffect(() => {
    if (textAreaRef) {
      // We need to reset the height momentarily to get the correct scrollHeight for the textarea
      textAreaRef.style.height = "0px";
      const scrollHeight = textAreaRef.scrollHeight;

      // Adding one pixel so that the the size doesn't change on the first input
      const fixedHeight = scrollHeight + 1;

      // We then set the height directly, outside of the render loop
      // Trying to set this with state or a ref will product an incorrect value
      textAreaRef.style.height = `${fixedHeight}px`;
    }
  }, [textAreaRef, value]);
};

export default useAutoresizeTextarea;
