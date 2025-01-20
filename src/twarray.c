#include <stdint.h>

// int array
typedef struct {
  void *loc;
  uint32_t size;
} twArr;

void twIntArrGet(twArr array, int i) {
  if (i >= 0 && i < array.size) {
    return array.loc[i];
  }
  return;
}

twArr make(int *loc, uint32_t size) {}
