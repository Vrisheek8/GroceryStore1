"use client";
import { useRouter } from "next/navigation";
import React, { useState } from "react";
import { IoIosAdd } from "react-icons/io";

const CartButton = ({ itemName, itemPrice }: { itemName: string, itemPrice: number }) => {
  const [cart, setCart] = useState<[string, number][]>([]);
  const router = useRouter();

  const handleAddToCart = () => {
    setCart((prevCart) => [...prevCart, [itemName, itemPrice]]);
    alert(`${itemName} has been added to the cart.`);
  };
  return (
    <>
    <button
      className="rounded-full p-1 bg-pink-500 hover:bg-pink-600"
      onClick={() => handleAddToCart()}
    >
      <IoIosAdd className="text-white text-3xl" />
    </button> 
    </>

  );
};
export default CartButton;
